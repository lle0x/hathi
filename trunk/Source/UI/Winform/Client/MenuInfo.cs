#region Copyright (c)2009 Hathi Project < http://hathi.sourceforge.net >
/*
* This file is part of Hathi Project
* Hathi Developers Team:
* andrewdev, beckman16, biskvit, elnomade_devel, ershyams, grefly, jpierce420,
* knocte, kshah05, manudenfer, palutz, ramone_hamilton, soudamini, writetogupta
*
* Hathi is a fork of Lphant Version 1.0 GPL
* Lphant Team
* Juanjo, 70n1, toertchn, FeuerFrei, mimontyf, finrold, jicxicmic, bladmorv,
* andrerib, arcange|, montagu, wins, RangO, FAV, roytam1, Jesse
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either
* version 2 of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*/
#endregion

using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Xml;

namespace Hathi.UI.Winform
{
/// <summary>
/// Summary description for MenuInfo.
/// </summary>
public class CMenuInfo
{
    private Menu.MenuItemCollection MenuItems;
    private string m_SearchString;
    private string m_eD2kLink;
    private string m_FileHash;
    private bool m_IsFilm;
    private Hashtable Actions;
    private MenuItem ShowAllLanguageMenuItem;
    private bool m_ShowAllLanguage;
    private string m_Language;
    private bool m_DisplayBar;
    private Menu m_Owner;

    public string SearchString
    {
        set
        {
//              if ((Path.GetExtension(value)==".avi")||
//                  (Path.GetExtension(value)==".mpeg")||
//                  (Path.GetExtension(value)==".mpg")||
//                  (Path.GetExtension(value)==".ogm")
//                  )
//                  m_IsFilm=true;
//              else
//                  m_IsFilm=false;
            m_IsFilm=CUtils.IsVideo(value);
            m_SearchString=CNameCleaner.Clean(value);
        }
    }

    public string Language
    {
        get
        {
            return m_Language;
        }
        set
        {
            m_Language=value;
        }
    }

    public bool ShowAllLanguage
    {
        get
        {
            return m_ShowAllLanguage;
        }
        set
        {
            m_ShowAllLanguage=value;
        }
    }

    public string eD2kLink
    {
        set
        {
            m_eD2kLink=value;
        }
    }

    public string FileHash
    {
        set
        {
            m_FileHash=value;
        }
    }

    public CMenuInfo(Menu owner)
    {
        m_CreateMenuInfo(owner);
    }

    private void m_CreateMenuInfo(Menu owner)
    {
        m_Language = m_GetLanguageFromCulture(HathiForm.preferences.GetString("Language"));
        m_ShowAllLanguage = HathiForm.preferences.GetBool("ShowAllLanguages",true);
        m_Owner = owner;
        Actions=new Hashtable();
        MenuItems=new Menu.MenuItemCollection(owner);
        MenuItem menu;
        try
        {
            ShowAllLanguageMenuItem = new MenuItem(HathiForm.Globalization["LBL_SHOW_ALL_LANGUAGES"],new EventHandler(ShowAllLanguageMenuItem_Click));
            ShowAllLanguageMenuItem.DefaultItem = true;
            ShowAllLanguageMenuItem.Checked = m_ShowAllLanguage;
            MenuItems.Add(ShowAllLanguageMenuItem);
            menu=new MenuItem("-");
            MenuItems.Add(menu);
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.StartupPath + Path.DirectorySeparatorChar + "webSearchs.xml");
            XmlNodeList nodes = doc.DocumentElement["Searchs"].ChildNodes;
            foreach (XmlElement el in nodes)
            {
                if (el.Name=="Search")
                {
                    if ((el.Attributes.Count>2) &&
                            (el.Attributes["SiteName"].InnerText!="")&&
                            (el.Attributes["URL"].InnerText!="")&&
                            (el.Attributes["Language"].InnerText!=""))
                    {
                        String NodeLanguage = el.Attributes["Language"].InnerText;
                        if (!m_ShowAllLanguage && NodeLanguage!=m_Language && NodeLanguage!="All")
                        {
                            m_DisplayBar = false;
                        }
                        else
                        {
                            m_DisplayBar = true;
                            menu=new MenuItem(el.Attributes["SiteName"].InnerText+"\t("+el.Attributes["Language"].InnerText+")",new EventHandler(OnItemClicked));
                            MenuItems.Add(menu);
                            Actions.Add(menu,el.Attributes["URL"].InnerText);
                        }
                    } else if (m_DisplayBar && el.Attributes["SiteName"].InnerText=="-")
                    {
                        menu=new MenuItem("-");
                        MenuItems.Add(menu);
                    }
                }
            }
            if (MenuItems[MenuItems.Count-1].Text=="-") MenuItems.RemoveAt(MenuItems.Count-1);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.ToString());
        }
    }
    private void OnItemClicked(object sender, System.EventArgs e)
    {
        if (m_SearchString.Length==0) return;
        string url=(string)Actions[sender];
        url=url.Replace("[CleanName]",m_SearchString);
        url=url.Replace("[HashId]",m_FileHash);
        url=url.Replace("[eD2kLink]",m_eD2kLink);
        if (m_IsFilm)
            url=url.Replace("[film]","film");
        else
            url=url.Replace("[film]","");
        if (url.Length>0)
            Process.Start(url);
    }

    private void ShowAllLanguageMenuItem_Click(object sender, EventArgs e)
    {
        m_ShowAllLanguage=!ShowAllLanguageMenuItem.Checked;
        HathiForm.preferences.SetProperty("ShowAllLanguages",m_ShowAllLanguage);
        ShowAllLanguageMenuItem.Click-=new EventHandler(ShowAllLanguageMenuItem_Click);
        MenuItems.Clear();
        m_CreateMenuInfo(m_Owner);
    }

    public void OnMenuInfoChange()
    {
        if (m_ShowAllLanguage!=HathiForm.preferences.GetBool("ShowAllLanguages",true))
        {
            ShowAllLanguageMenuItem.Click-=new EventHandler(ShowAllLanguageMenuItem_Click);
            MenuItems.Clear();
            m_CreateMenuInfo(m_Owner);
            return;
        }
        if (m_Language!=m_GetLanguageFromCulture(HathiForm.preferences.GetString("Language")))
        {
            ShowAllLanguageMenuItem.Click-=new EventHandler(ShowAllLanguageMenuItem_Click);
            MenuItems.Clear();
            m_CreateMenuInfo(m_Owner);
            return;
        }
    }

    private string m_GetLanguageFromCulture(string CultureInfo)
    {
        return CultureInfo.Split("-".ToCharArray())[0];
    }
}
}

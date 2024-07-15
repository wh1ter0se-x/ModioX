﻿using ArisenStudio.Database;
using ArisenStudio.Extensions;
using ArisenStudio.Forms.Windows;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ArisenStudio.Models.Database
{
    public class AppsData
    {
        /// <summary>
        /// Get the apps from the database.
        /// </summary>
        public List<AppItemData> Mods { get; set; }

        /// <summary>
        /// Get the supported firmwares from all of the mods.
        /// </summary>
        /// <returns> Firmwares Supported </returns>
        public List<string> Versions
        {
            get
            {
                List<string> firmwares = Mods.SelectMany(x => x.Versions).Where(x => !x.EqualsIgnoreCase("-")).Distinct().ToList();
                firmwares.Sort();
                return firmwares.Distinct().ToList();
            }
        }

        /// <summary>
        /// Get all of the apps matching the specified filters.
        /// </summary>
        /// <param name="categoriesData"></param>
        /// <param name="categoryId"></param>
        /// <param name="name"></param>
        /// <param name="fwVersion"></param>
        /// <param name="version"></param>
        /// <param name="favorites"></param>
        /// <returns></returns>
        public List<AppItemData> GetAppItems(CategoriesData categoriesData, string categoryId, string name,  string version, string fwVersion, bool favorites = false)
        {
            if (favorites)
            {
                return Mods.Where(x =>
                    x.GetCategoryType(categoriesData) == CategoryType.Application &&
                    MainWindow.Settings.FavoriteMods.Exists(y => y.ModId == x.Id) &&
                    x.GetCategoryName(categoriesData).EqualsIgnoreCase(categoryId) &&
                    //x.GetCategoryType(categoriesData) == CategoryType.Homebrew &&
                    (categoryId.IsNullOrEmpty() ? x.CategoryId.ContainsIgnoreCase(categoryId) : x.CategoryId.EqualsIgnoreCase(categoryId)) &&
                    x.Name.ContainsIgnoreCase(name) &&
                    x.FirmwareVersions.Exists(y => y.ContainsIgnoreCase(fwVersion)) &&
                    x.Versions.ToArray().AnyContainsIgnoreCase(version))
                    .ToList();
            }
            else
            {
                return Mods.Where(x =>
                    x.GetCategoryType(categoriesData) == CategoryType.Application &&
                    (categoryId.IsNullOrEmpty() ? x.CategoryId.ContainsIgnoreCase(categoryId) : x.CategoryId.EqualsIgnoreCase(categoryId)) &&
                    //x.GetCategoryName(categoriesData).EqualsIgnoreCase(categoryId) &&
                    //(categoryId.IsNullOrEmpty() ? x.CategoryId.ContainsIgnoreCase(categoryId) : x.CategoryId.EqualsIgnoreCase(categoryId)) &&
                    x.Name.ContainsIgnoreCase(name) &&
                    x.FirmwareVersions.Exists(y => y.ContainsIgnoreCase(fwVersion)) &&
                    //x.Version.ContainsIgnoreCase(version) &&
                    //x.Region.ContainsIgnoreCase(region) &&
                    x.Versions.ToArray().AnyContainsIgnoreCase(version))
                    .ToList();
            }
        }

        /// <summary>
        /// Get the <see cref="AppItemData" /> matching the specified <see cref="AppItemData.Id" />.
        /// </summary>
        /// <param name="id">
        /// <see cref="AppItemData.Id" />
        /// </param>
        /// <returns> Mod details for the <see cref="AppItemData.Id" /> </returns>
        public AppItemData GetModById(Platform platform, int id)
        {
            return Mods.FirstOrDefault(modItem => modItem.GetPlatform() == platform && modItem.Id.Equals(id));
        }
    }
}
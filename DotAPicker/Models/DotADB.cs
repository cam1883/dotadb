﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DotAPicker.Models
{
    public class DotADB
    {
        private const string DB_PATH_SETTING = "DBPath";
        public string CurrentPatch { get; set; } = "7.06"; //TODO: get this from a "current patch" setting

        public List<Hero> Heroes { get; set; } = new List<Hero>();
        public List<Tip> Tips { get; set; } = new List<Tip>();
        public List<Relationship> Relationships { get; set; } = new List<Relationship>();

        public HeroDetailViewModel HeroDetails(int ID) => new HeroDetailViewModel() {
            Hero = Heroes.FirstOrDefault(h => h.ID == ID),
            Tips = Tips.Where(t => t.HeroID == ID),
            Relationships = Relationships.Where(r => r.IncludesHero(ID))
        };

        /// <summary>
        /// Defaults to the path in the web config
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static DotADB Load(string filename = null)
        {
            filename = filename ?? ConfigurationManager.AppSettings[DB_PATH_SETTING];

            //first run logic
            if (!File.Exists(filename)) new DotADB().Save();

            var s = new XmlSerializer(typeof(DotADB));
            var fs = new FileStream(filename, FileMode.Open); //todo: what if file can't be accessed, is invalid, etc.
            var retVal = (DotADB)s.Deserialize(fs);
            fs.Close();
            return retVal;
        }

        /// <summary>
        /// Defaults to the path in the web config
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename = null)
        {
            filename = filename ?? ConfigurationManager.AppSettings[DB_PATH_SETTING];

            var s = new XmlSerializer(typeof(DotADB));
            if (File.Exists(filename)) File.Delete(filename); //todo: something more robust
            var fs = new FileStream(filename, FileMode.OpenOrCreate);
            s.Serialize(fs, this);
            fs.Close();
        }
    }
}
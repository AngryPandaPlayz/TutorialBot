﻿// Ignore Spelling: warningcount

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialBot.DataBase
{
    public sealed class JSONDataHandler
    {
        public string CUSTOM_PREFIX { get; set; }

        public class PrivateVCUserData
        {
            public ulong ID { get; set; }

            public ulong GuildID { get; set; }

            public ulong CategoryID { get; set; }

            public ulong ChannelID { get; set; }

            public ulong RoleID { get; set; }

            public PrivateVCUserData(ulong id, ulong guildID, ulong categoryID, ulong channelID, ulong roleID)
            {
                ID = id;
                GuildID = guildID;
                CategoryID = categoryID;
                ChannelID = channelID;
                RoleID = roleID;
            }
        }

        public class GuildData
        {
            public ulong ID { get; set; }

            public string Prefix { get; set; }

            public GuildData(ulong id, string prefix)
            {
                ID = id;
                Prefix = prefix;
            }
        }

        public class ChannelData
        {
            public ulong ID { get; set; }

            public ulong GuildID { get; set; }

            public ChannelData(ulong id, ulong guildID)
            {
                ID = id;
                GuildID = guildID;
            }
        }

        public class RoleData
        {
            public ulong ID { get; set; }

            public ulong GuildID { get; set; }

            public RoleData(ulong id, ulong guildID)
            {
                ID = id;
                GuildID = guildID;
            }
        }

        public class MessageData
        {
            public ulong ID { get; set; }

            public ulong GuildID { get; set; }

            public MessageData(ulong id, ulong guildID)
            {
                ID = id;
                GuildID = guildID;
            }
        }

        public class CategoryData
        {
            public ulong ID { get; set; }

            public ulong GuildID { get; set; }

            public CategoryData(ulong id, ulong guildID)
            {
                ID = id;
                GuildID = guildID;
            }
        }

        public enum DATA_TYPES
        {
            USERVC,
            GUILD,
            CATEGORY,
            CHANNEL,
            ROLE,
            MESSAGE
        }

        public enum DATA_CATEGORIES
        {
            PrivateVCUserData,
            GuildData,
            ChannelData,
            RoleData,
            MessageData,
            CategoryData
        }

        public DATA_TYPES GetTypes(DATA_TYPES type)
        {
            return type;
        }

        public DATA_CATEGORIES GetCategories(DATA_CATEGORIES category)
        {
            return category;
        }


        public string CreatePathIfNotExists(DATA_TYPES type, DATA_CATEGORIES category)
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\DataBase\\{type}\\{category}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Creating the following Path: " + $"{AppDomain.CurrentDomain.BaseDirectory}\\DataBase\\{type}\\{category}");
                Console.WriteLine($"Created all paths for {AppDomain.CurrentDomain.BaseDirectory}\\DataBase\\{type}\\{category}.");
                return path;
            }
            else if (Directory.Exists(path))
            {
                Console.WriteLine("Loading the following Path: " + $"{AppDomain.CurrentDomain.BaseDirectory}\\DataBase\\{type}\\{category}");
                Console.WriteLine($"All Paths Are Loaded for {AppDomain.CurrentDomain.BaseDirectory}\\DataBase\\{type}\\{category}.");
                return path;
            }
            return null;
        }

        public static string GetJSONFilePath(DATA_TYPES type, DATA_CATEGORIES category, ulong id)
        {
            Console.WriteLine("Getting JSON File Path: " + $"{AppDomain.CurrentDomain.BaseDirectory}\\DataBase\\{type}\\{category}\\{id}.json");
            return $"DataBase/{type}/{category}/{id}.json";
        }

        public async Task <PrivateVCUserData> SavePrivateVCUserDataToJSON(PrivateVCUserData data, ulong userID)
        {
            string path = GetJSONFilePath(DATA_TYPES.USERVC, DATA_CATEGORIES.PrivateVCUserData, userID);
            if (!File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(data);
                await File.WriteAllTextAsync(path, json);
                Console.WriteLine("Saved Private VC User Data to JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task <GuildData> SaveGuildDataToJSON(GuildData data, ulong guildID)
        {
            string path = GetJSONFilePath(DATA_TYPES.GUILD, DATA_CATEGORIES.GuildData, guildID);
            if (!File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(data);
                await File.WriteAllTextAsync(path, json);
                Console.WriteLine("Saved Guild Data to JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task <ChannelData> SaveChannelDataToJSON(ChannelData data, ulong channelID)
        {
            string path = GetJSONFilePath(DATA_TYPES.CHANNEL, DATA_CATEGORIES.ChannelData, channelID);
            if(!File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(data);
                await File.WriteAllTextAsync(path, json);
                Console.WriteLine("Saved Channel Data to JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task <RoleData> SaveRoleDataToJSON(RoleData data, ulong roleID)
        {
            string path = GetJSONFilePath(DATA_TYPES.ROLE, DATA_CATEGORIES.RoleData, roleID);
            if (!File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(data);
                await File.WriteAllTextAsync(path, json);
                Console.WriteLine("Saved Role Data to JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task <MessageData> SaveMessageDataToJSON(MessageData data, ulong messageID)
        {
            string path = GetJSONFilePath(DATA_TYPES.MESSAGE, DATA_CATEGORIES.MessageData, messageID);
            if (!File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(data);
                await File.WriteAllTextAsync(path, json);
                Console.WriteLine("Saved Message Data to JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task <CategoryData> SaveCategoryDataToJSON(CategoryData data, ulong categoryID)
        {
            string path = GetJSONFilePath(DATA_TYPES.CATEGORY, DATA_CATEGORIES.CategoryData, categoryID);
            if(!File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(data);
                await File.WriteAllTextAsync(path, json);
                Console.WriteLine("Saved Category Data to JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task<GuildData> SetCustomPrefix(string prefix, ulong guildID)
        {
            string path = GetJSONFilePath(DATA_TYPES.GUILD, DATA_CATEGORIES.GuildData, guildID);
            if (!File.Exists(path))
            {
                return null;
            }
            else if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                GuildData data = JsonConvert.DeserializeObject<GuildData>(json);
                data.Prefix = prefix;
                string newjson = JsonConvert.SerializeObject(data);
                await File.WriteAllTextAsync(path, newjson);
                Console.WriteLine("Set Custom Prefix: " + path);
                return data;
            }
            return null;
        }

        public async Task<PrivateVCUserData> GetAllPrivateVcUserDataFromJSON(ulong userID)
        {
            string path = GetJSONFilePath(DATA_TYPES.USERVC, DATA_CATEGORIES.PrivateVCUserData, userID);
            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                PrivateVCUserData data = JsonConvert.DeserializeObject<PrivateVCUserData>(json);
                Console.WriteLine("Got All Private VC User Data from JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task<GuildData> GetAllGuildDataFromJSON(ulong guildID)
        {
            string path = GetJSONFilePath(DATA_TYPES.GUILD, DATA_CATEGORIES.GuildData, guildID);
            if(File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                GuildData data = JsonConvert.DeserializeObject<GuildData>(json);
                Console.WriteLine("Got All Guild Data from JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task<ChannelData> GetAllChannelDataFromJSON(ulong channelID)
        {
            string path = GetJSONFilePath(DATA_TYPES.CHANNEL, DATA_CATEGORIES.ChannelData, channelID);
            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                ChannelData data = JsonConvert.DeserializeObject<ChannelData>(json);
                Console.WriteLine("Got All Channel Data from JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task<RoleData> GetAllRoleDataFromJSON(ulong roleID)
        {
            string path = GetJSONFilePath(DATA_TYPES.ROLE, DATA_CATEGORIES.RoleData, roleID);
            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                RoleData data = JsonConvert.DeserializeObject<RoleData>(json);
                Console.WriteLine("Got All Role Data from JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task<MessageData> GetAllMessageDataFromJSON(ulong messageID)
        {
            string path = GetJSONFilePath(DATA_TYPES.MESSAGE, DATA_CATEGORIES.MessageData, messageID);
            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                MessageData data = JsonConvert.DeserializeObject<MessageData>(json);
                Console.WriteLine("Got All Message Data from JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task<CategoryData> GetAllCategoryDataFromJSON(ulong categoryID)
        {
            string path = GetJSONFilePath(DATA_TYPES.CATEGORY, DATA_CATEGORIES.CategoryData, categoryID);
            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                CategoryData data = JsonConvert.DeserializeObject<CategoryData>(json);
                Console.WriteLine("Got All Category Data from JSON: " + path);
                return data;
            }
            return null;
        }

        public async Task DeleteGuildData(ulong guildID)
        {
            string path = GetJSONFilePath(DATA_TYPES.GUILD, DATA_CATEGORIES.GuildData, guildID);
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine("Deleted Guild Data: " + path);
            }
        }

        public async Task DeleteChannelData(ulong channelID)
        {
            string path = GetJSONFilePath(DATA_TYPES.CHANNEL, DATA_CATEGORIES.ChannelData, channelID);
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine("Deleted Channel Data: " + path);
            }
        }

        public async Task DeleteRoleData(ulong roleID)
        {
            string path = GetJSONFilePath(DATA_TYPES.ROLE, DATA_CATEGORIES.RoleData, roleID);
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine("Deleted Role Data: " + path);
            }
        }

        public async Task DeleteMessageData(ulong messageID)
        {
            string path = GetJSONFilePath(DATA_TYPES.MESSAGE, DATA_CATEGORIES.MessageData, messageID);
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine("Deleted Message Data: " + path);
            }
        }

        public async Task DeleteCategoryData(ulong categoryID)
        {
            string path = GetJSONFilePath(DATA_TYPES.CATEGORY, DATA_CATEGORIES.CategoryData, categoryID);
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine("Deleted Category Data: " + path);
            }
        }

        public async Task DeletePrivateVCUserData(ulong userID)
        {
            string path = GetJSONFilePath(DATA_TYPES.USERVC, DATA_CATEGORIES.PrivateVCUserData, userID);
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine("Deleted Private VC User Data: " + path);
            }
        }

    }
}

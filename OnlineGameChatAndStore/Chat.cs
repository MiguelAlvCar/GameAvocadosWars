using System;
using System.Collections.Generic;
using System.Text;
using DTO_Models;

namespace OnlineGameChatAndStore
{
    public class Chat
    {
        static public event Action<ChatEntry> EntryAdded;
        public void AddChatEntry(ChatEntry entry)
        {
            ChatList.Add(entry);
            EntryAdded(entry);
        }
        public ChatEntry[] GetChatList()
        {
            return ChatList.ToArray();
        }
        private List<ChatEntry> ChatList { set; get; } = new List<ChatEntry>();
    }

    public class ChatEntry
    {
        public ChatEntry (string entry, PlayerDTO particularPlayer, bool isHostPlayer)
        {
            Entry = entry;
            ParticularPlayer = particularPlayer;
            IsHostPlayer = isHostPlayer;
        }
        public string Entry { set; get; }
        public PlayerDTO ParticularPlayer { set; get; }
        public bool IsHostPlayer { set; get; }
    }
}

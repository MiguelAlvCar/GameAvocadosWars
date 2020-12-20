using System;
using System.Collections.Generic;
using System.Text;
using DTO_Models;
using System.Net;

namespace BasicElements.AbstractClasses
{
    [Serializable()]
    public class SerializablePlayerDTO
    {
        public static PlayerDTO ConvertToPlayerDTO (SerializablePlayerDTO player)
        {
            PlayerDTO playerDTO = new PlayerDTO(player.Name, player.Ability, player.ID);
            playerDTO.Email = player.Email;
            playerDTO.Battles = player.Battles;
            playerDTO.WonBattles = player.WonBattles;
            playerDTO.GlobalIP = player.GlobalIP;
            playerDTO.LocalIPs = player.LocalIPs;
            return playerDTO;
        }

        public static SerializablePlayerDTO ConvertDTOToSerializable(PlayerDTO playerDTO)
        {
            SerializablePlayerDTO player = new SerializablePlayerDTO();
            player.Name = playerDTO.Name;
            player.Ability = playerDTO.Ability;
            player.ID = playerDTO.ID;
            player.Email = playerDTO.Email;
            player.Battles = playerDTO.Battles;
            player.WonBattles = playerDTO.WonBattles;
            player.GlobalIP = playerDTO.GlobalIP;
            player.LocalIPs = playerDTO.LocalIPs;
            return player;
        }

        public string Email { set; get; }
        public string ID { set; get; }
        public string Name { set; get; }
        public double Ability { set; get; }
        public int Battles { set; get; }
        public int WonBattles { set; get; }
        public ICollection<LocalIPDTO> LocalIPs { set; get; }
        public string GlobalIP { set; get; }
    }
}

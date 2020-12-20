using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace OnlineGameChatAndStore
{
    static public class EntryInFlowDocument
    {
        static public void Write(ChatEntry entry, System.Windows.Documents.List chatList)
        {
            ListItem newItem = new ListItem();
            Paragraph paragraph = new Paragraph();
            if (entry.ParticularPlayer != null)
            {
                Run player = new Run(entry.ParticularPlayer.Name + ": ");
                Span span = new Span(player);
                span.FontWeight = FontWeights.Bold;
                if (entry.IsHostPlayer)
                    span.Foreground = Brushes.Blue;
                else
                    span.Foreground = Brushes.Red;
                paragraph.Inlines.Add(span);
            }
            else
            {
                Run player = new Run("MiguelMal: ");
                Span span = new Span(player);
                span.FontWeight = FontWeights.Bold;
                span.FontWeight = FontWeights.Bold;
                if (entry.IsHostPlayer)
                    span.Foreground = Brushes.Blue;
                else
                    span.Foreground = Brushes.Red;
                paragraph.Inlines.Add(span);
            }

            Run message = new Run(entry.Entry);
            paragraph.Inlines.Add(message);
            newItem.Blocks.Add(paragraph);
            chatList.ListItems.Add(newItem);

        }
    }
}

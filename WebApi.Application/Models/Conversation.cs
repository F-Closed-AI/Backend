using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace WebApi.Application.Models
{
    public class Conversation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Subject { get; set; }

        public List<ConversationPhrase> ConversationPhrases { get; set;}

        public void ParseConversationContent(string responseContent)
        {
            var responseJson = JsonConvert.DeserializeObject<dynamic>(responseContent);
            string answer = responseJson.output.answer;

            ConversationPhrases = ParsePhrases(answer);
        }

        private List<ConversationPhrase> ParsePhrases(string answer)
        {
            var phrases = new List<ConversationPhrase>();
            var matches = Regex.Matches(answer, @"(\w+): (.+?)(?=\n\n(\w+:)|$)");

            foreach (Match match in matches)
            {
                phrases.Add(new ConversationPhrase
                {
                    Name = match.Groups[1].Value,
                    Content = match.Groups[2].Value
                });
            }

            return phrases;
        }


    }
}


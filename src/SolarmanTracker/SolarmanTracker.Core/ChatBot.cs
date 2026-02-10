using Telegram.Bot;
using Telegram.Bot.Types;

namespace SolarmanTracker.Core
{
    public sealed class ChatBot
    {
        private TelegramBotClient client;

        public ChatBot(string token)
        {
            client = new TelegramBotClient(token);
        }

        public async Task Post(string message, string chatId)
        {
            await client.SendMessage(chatId, message, Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public async Task PostImage(string fileName, string text, string chatId)
        {
            Message message;

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var finalPath = Path.Combine(basePath, fileName);

            using (var stream = File.OpenRead(finalPath))
            {
                message = await postImage(stream, text, chatId);
            }
        }

        public async Task PostImageBytes(byte[] buffer, string text, string chatId)
        {
            Message message;

            using (var stream = new MemoryStream(buffer))
            {
                message = await postImage(stream, text, chatId);
            }
        }

        private async Task<Message> postImage(Stream stream, string text, string chatId)
        {
            return await client.SendPhoto(
                chatId: chatId,
                photo: stream,
                caption: text
            );
        }
    }
}

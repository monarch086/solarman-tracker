using Amazon.Lambda.Core;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SolarmanTracker.Core
{
    public sealed class ChatBot
    {
        private readonly ILambdaLogger logger;
        private readonly TelegramBotClient client;
        private string stage = string.Empty;

        private const string APP_NAME = "SolarmanTracker";
        private const string PERSONAL_CHAT_ID = "38627946";

        public ChatBot(string stage, string token, ILambdaLogger logger)
        {
            this.stage = stage;
            client = new TelegramBotClient(token);
            this.logger = logger;
        }

        public async Task Post(string message, string chatId)
        {
            await client.SendMessage(chatId, message, Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public async Task PostWarning(string message)
        {
            logger.LogWarning(message);

            var warningMessage = $"⚠️ <b>Attention!</b>\n{APP_NAME}-{stage}\n{message}";
            await client.SendMessage(PERSONAL_CHAT_ID, warningMessage, Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public async Task PostError(string message)
        {
            logger.LogError(message);

            var errorMessage = $"❌ <b>Error!</b>\n{APP_NAME}-{stage}\n{message}";
            await client.SendMessage(PERSONAL_CHAT_ID, errorMessage, Telegram.Bot.Types.Enums.ParseMode.Html);
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

using System.Text.Json;

namespace BEBoxchat.Util
{
    public class Util
    {
        public static string ExtractTargetUser(string message)
        {
            try
            {
                var json = JsonDocument.Parse(message);
                if (json.RootElement.TryGetProperty("userId", out var targetUser))
                {
                    return targetUser.GetString();
                }
            }
            catch (JsonException)
            {
                // Log lỗi nếu cần
            }

            return null; // Trả về null nếu không tìm thấy targetUser
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtMaui.Helpers
{
    public class JwtTokenHelper
    {
        public string Token { get; set; } = string.Empty;
        string jwtFilename = "jwt.data";

        public async Task ReadAsync()
        {
            string file = Path.Combine(FileSystem.Current.AppDataDirectory, jwtFilename);
            Token = "";
            try
            {
                Token = await File.ReadAllTextAsync(file);
            }
            catch { }
        }

        public async Task WriteAsync(string token)
        {
            string file = Path.Combine(FileSystem.Current.AppDataDirectory, jwtFilename);
            Token = token;
            try
            {
                await File.WriteAllTextAsync(file, token);
            }
            catch { }
        }
    }
}

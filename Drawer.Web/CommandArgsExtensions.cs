
namespace Drawer.Web
{
    public static class CommandArgsExtensions
    {
        /// <summary>
        /// 커맨드라인 명령 인자를 처리한다.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="args"></param>
        public static void HandleArgs(this ConfigurationManager configuration, string[] args)
        {
            var keyValuePairs = args.Select(arg => arg.Split("=")
                    .Select(x => x.Trim())
                    .ToArray())
                .Where(pair => pair.Length == 2)
                .ToList();

            foreach(var pair in keyValuePairs)
            {
                configuration.AddInMemoryCollection(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>(pair[0], pair[1])
                });
            }
        }
    }
}

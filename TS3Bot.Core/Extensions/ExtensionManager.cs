using System;
using System.Collections.Generic;
using System.Text;
using TS3Bot.Core.Libraries;

namespace TS3Bot.Core.Extensions
{
    public sealed class ExtensionManager
    {
        private IList<Extension> extensions;

        // All registered libraries
        private IDictionary<string, Library> libraries;

        public ExtensionManager()
        {
            // Initialize
            extensions = new List<Extension>();
        }

        /// <summary>
        /// Registers the specified library
        /// </summary>
        /// <param name="name"></param>
        /// <param name="library"></param>
        public void RegisterLibrary(string name, Library library)
        {
            if (libraries.ContainsKey(name))
                //Interface.TS3Bot.LogError("An extension tried to register an already registered library: " + name);
                new NotImplementedException();
            else
                libraries[name] = library;
        }

        /// <summary>
        /// Gets all library names
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetLibraries() => libraries.Keys;

        /// <summary>
        /// Gets the library by the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Library GetLibrary(string name)
        {
            Library lib;
            return !libraries.TryGetValue(name, out lib) ? null : lib;
        }
    }
}

﻿using System;
using Txt;
using Txt.ABNF;
using Uri.dec_octet;

namespace Uri.IPv4address
{
    public class IPv4AddressLexerFactory : ILexerFactory<IPv4Address>
    {
        private readonly ILexerFactory<DecimalOctet> decimaOctetLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public IPv4AddressLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<DecimalOctet> decimaOctetLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }

            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }

            if (decimaOctetLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(decimaOctetLexerFactory));
            }

            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.decimaOctetLexerFactory = decimaOctetLexerFactory;
        }

        public ILexer<IPv4Address> Create()
        {
            // dec-octet
            var a = decimaOctetLexerFactory.Create();

            // "."
            var b = terminalLexerFactory.Create(@".", StringComparer.Ordinal);

            // dec-octet "." dec-octet "." dec-octet "." dec-octet
            var c = concatenationLexerFactory.Create(a, b, a, b, a, b, a);

            // IPv4address
            return new IPv4AddressLexer(c);
        }
    }
}
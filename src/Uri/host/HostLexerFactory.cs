﻿using System;
using Txt;
using Txt.ABNF;
using Uri.IPv4address;
using Uri.IP_literal;
using Uri.reg_name;

namespace Uri.host
{
    public class HostLexerFactory : ILexerFactory<Host>
    {
        private readonly IAlternativeLexerFactory alternativeLexerFactory;

        private readonly ILexerFactory<IPLiteral> ipLiteralLexerFactory;

        private readonly ILexerFactory<IPv4Address> ipv4AddressLexerFactory;

        private readonly ILexerFactory<RegisteredName> registeredNameLexerFactory;

        public HostLexerFactory(
            IAlternativeLexerFactory alternativeLexerFactory,
            ILexerFactory<IPLiteral> ipLiteralLexerFactory,
            ILexerFactory<IPv4Address> ipv4AddressLexerFactory,
            ILexerFactory<RegisteredName> registeredNameLexerFactory)
        {
            if (alternativeLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternativeLexerFactory));
            }

            if (ipLiteralLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(ipLiteralLexerFactory));
            }

            if (ipv4AddressLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(ipv4AddressLexerFactory));
            }

            if (registeredNameLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(registeredNameLexerFactory));
            }

            this.alternativeLexerFactory = alternativeLexerFactory;
            this.ipLiteralLexerFactory = ipLiteralLexerFactory;
            this.ipv4AddressLexerFactory = ipv4AddressLexerFactory;
            this.registeredNameLexerFactory = registeredNameLexerFactory;
        }

        public ILexer<Host> Create()
        {
            var ipliteral = ipLiteralLexerFactory.Create();
            var ipv4 = ipv4AddressLexerFactory.Create();
            var regName = registeredNameLexerFactory.Create();
            var innerLexer = alternativeLexerFactory.Create(ipliteral, ipv4, regName);
            return new HostLexer(innerLexer);
        }
    }
}
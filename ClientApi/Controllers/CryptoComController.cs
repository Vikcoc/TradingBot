﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Traders.CryptoCom;
using TradingWebSocket.BaseTrader;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoComController : ControllerBase
    {
        private readonly CryptoComTrader _cryptoComTrader;

        public CryptoComController(CryptoComTrader cryptoComTrader)
        {
            _cryptoComTrader = cryptoComTrader;
        }

        [HttpGet("Connect")]
        public async Task<IActionResult> ConnectAsync()
        {
            _cryptoComTrader.PriceUpdate += d =>
            {
                Console.WriteLine("Price: " + d);
                return Task.CompletedTask;
            };
            _cryptoComTrader.BuyAvailableUpdate += d =>
            {
                Console.WriteLine("Buy: " + d);
                return Task.CompletedTask;
            };
            _cryptoComTrader.SellAvailableUpdate += d =>
            {
                Console.WriteLine("Sell: " + d);
                return Task.CompletedTask;
            };

            await _cryptoComTrader.Start(Trades.BtcUsd);
            return Ok();
        }
    }
}

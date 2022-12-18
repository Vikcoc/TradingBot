using Microsoft.AspNetCore.Mvc;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly Timer _timer;

        public TestsController()
        {
            _timer = new Timer(600); // Wait for one minute before setting the threshold values
            _timer.Elapsed += OnTimerElapsed;
        }

        private static void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("a");
        }

        [HttpGet("StartTimer")]
        public void StartTimer()
        {
            _timer.Start();
        }
    }
}

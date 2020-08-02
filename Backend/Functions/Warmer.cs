using Microsoft.Azure.WebJobs;

namespace TrackerSafe.Backend.Functions
{
  public class Warmer
  {
    [FunctionName("Warmer")]
    public static void WarmUp([TimerTrigger("0 */15 * * * *")]TimerInfo timer)
    {
      //Do Nothing, just keep the functions host warm for SPEEDZ! :)
    }
  }
}

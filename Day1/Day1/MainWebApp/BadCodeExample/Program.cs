

static class Factory
{
    public static IUsersManager GetIUserManagerInstance()
    {
        return new UsersSqlManager();
    }
}

class Manager
{
    private IUsersManager usersManager;

    private HIDGateManager gateManager = new HIDGateManager();
    private NxpCardReader nxpCardReader = new NxpCardReader();

    public Manager(IUsersManager usersManager)
    {
        this.usersManager = usersManager;
    }

    public async Task Start()
    {
        while (true)
        {
            var tz = await  nxpCardReader.GetNumber();
            if (await usersManager.IsUserValid(tz))
            {
                await gateManager.OpenGateFor1();
            }
            else
            {
                await Console.Out.WriteLineAsync("please wait (4ever)" );
            }
        }
    }
}

interface IUsersManager
{
    Task<bool> IsUserValid(string tzNumber);
}

class UsersSqlManager : IUsersManager
{
    public async Task<bool> IsUserValid(string tzNumber)
    {
        return false;
    }
}

class HIDGateManager
{
    public async Task OpenGateFor1()
    {
        await Task.Delay(2000);
    }

}
class NxpCardReader
{
    public async Task<string> GetNumber()
    {
        return "";
    }
}

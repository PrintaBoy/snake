public class CommandInvoker
{    
    public static void ExecuteCommand(ICommand command) // this method exist to only execute commands given to it
    {
        command.Execute();
    }
}

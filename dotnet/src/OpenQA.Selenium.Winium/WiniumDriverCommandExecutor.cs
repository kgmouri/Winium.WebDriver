namespace OpenQA.Selenium.Winium
{
    #region using

    using System;
    using System.Threading.Tasks;

    #endregion

    public class WiniumDriverCommandExecutor : ICommandExecutor
    {
        #region Fields

        private ICommandExecutor internalExecutor;

        private WiniumDriverService service;

        #endregion

        #region Constructors and Destructors

        public WiniumDriverCommandExecutor(WiniumDriverService driverService, TimeSpan commandTimeout)
        {
            this.service = driverService;
            this.internalExecutor = CommandExecutorFactory.GetHttpCommandExecutor(driverService.ServiceUrl, commandTimeout);
        }

        #endregion

        #region Public Properties

        /// <see cref="ICommandExecutor.TryAddCommand"/>
        public bool TryAddCommand(string commandName, CommandInfo info)
        {
            return this.internalExecutor.TryAddCommand(commandName, info);
        }

        public void Dispose()
        {
            if (this.service.IsRunning is true)
            {
                this.service.Dispose();
                while (this.service.ProcessId != 0)
                {
                    System.Threading.Thread.Sleep(200);
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        public Response Execute(Command commandToExecute)
        {
            return Task.Run(() => ExecuteAsync(commandToExecute)).GetAwaiter().GetResult();
        }

        /// <see cref="ICommandExecutor.ExecuteAsync"/>
        public async Task<Response> ExecuteAsync(Command commandToExecute)
        {
            if (commandToExecute == null)
            {
                throw new ArgumentNullException("commandToExecute", "Command may not be null");
            }

            if (commandToExecute.Name == DriverCommand.NewSession)
            {
                this.service.Start();
            }

            try
            {
                return await this.internalExecutor.ExecuteAsync(commandToExecute).ConfigureAwait(false);
            }
            finally
            {
                if (commandToExecute.Name == DriverCommand.Quit)
                {
                    this.service.Dispose();
                }
            }
        }

        #endregion
    }
}

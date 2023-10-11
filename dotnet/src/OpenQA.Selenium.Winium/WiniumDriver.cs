namespace OpenQA.Selenium.Winium
{
    #region using

    using System;

    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    #endregion

    /// <summary>
    ///  Provides a mechanism to write tests using Winium driver.
    /// </summary>
    /// <example>
    /// <code>
    /// [TestFixture]
    /// public class Testing
    /// {
    ///     private IWebDriver driver;
    ///     <para></para>
    ///     [SetUp]
    ///     public void SetUp()
    ///     {
    ///         var options = new DesktopOptions { ApplicationPath = @"‪C:\Windows\System32\notepad.exe" };
    ///         driver = new WiniumDriver(options);
    ///     }
    ///     <para></para>
    ///     [Test]
    ///     public void TestGoogle()
    ///     {
    ///        /*
    ///         *   Rest of the test
    ///         */
    ///     }
    ///     <para></para>
    ///     [TearDown]
    ///     public void TearDown()
    ///     {
    ///         driver.Quit();
    ///     } 
    /// }
    /// </code>
    /// </example>
    public class WiniumDriver : WebDriver, IFindsWiniumElement
    {
        #region Fields

        private WiniumElementFactory elementFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WiniumDriver"/> class using the specified path
        /// to the directory containing Winium.Driver executible file and options.
        /// </summary>
        /// <param name="winiumDriverDirectory">
        /// The full path to the directory containing Winium.Driver executible.
        /// </param>
        /// <param name="options">
        /// The <see cref="DesktopOptions"/> to be used with the Winium driver.
        /// </param>
        public WiniumDriver(string winiumDriverDirectory, DriverOptions options)
            : this(winiumDriverDirectory, options, WebDriver.DefaultCommandTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WiniumDriver"/> class using the specified path
        /// to the directory containing Winium.Driver executible file, options, and command timeout.
        /// </summary>
        /// <param name="winiumDriverDirectory">
        /// The full path to the directory containing Winium.Driver executible file.
        /// </param>
        /// <param name="options">
        /// The <see cref="DesktopOptions"/> to be used with the Winium driver.
        /// </param>
        /// <param name="commandTimeout">
        /// The maximum amount of time to wait for each command.
        /// </param>
        public WiniumDriver(string winiumDriverDirectory, DriverOptions options, TimeSpan commandTimeout)
            : this(CreateDefaultService(options.GetType(), winiumDriverDirectory), options, commandTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WiniumDriver"/> class using the specified 
        /// <see cref="WiniumDriverService"/> and options.
        /// </summary>
        /// <param name="service">The <see cref="WiniumDriverService"/> to use.</param>
        /// <param name="options">The <see cref="DesktopOptions"/> used to initialize the driver.</param>
        public WiniumDriver(WiniumDriverService service, DriverOptions options)
            : this(service, options, WebDriver.DefaultCommandTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WiniumDriver"/> class using the specified <see cref="WiniumDriverService"/>.
        /// </summary>
        /// <param name="service">The <see cref="WiniumDriverService"/> to use.</param>
        /// <param name="options">The <see cref="DriverOptions"/> object to be used with the Winium driver.</param>
        /// <param name="commandTimeout">The maximum amount of time to wait for each command.</param>
        public WiniumDriver(WiniumDriverService service, DriverOptions options, TimeSpan commandTimeout)
            : base(new WiniumDriverCommandExecutor(service, commandTimeout), options.ToCapabilities())
        {
            this.InitWiniumDriverCommands();
            elementFactory = new WiniumElementFactory(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WiniumDriver"/> class using the specified remote address and options.
        /// </summary>
        /// <param name="remoteAddress">URI containing the address of the WiniumDriver remote server (e.g. http://127.0.0.1:4444/wd/hub).</param>
        /// <param name="options">The <see cref="DriverOptions"/> object to be used with the Winium driver.</param>
        public WiniumDriver(Uri remoteAddress, DriverOptions options)
            : this(remoteAddress, options, WebDriver.DefaultCommandTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WiniumDriver"/> class using the specified remote address, desired capabilities, and command timeout.
        /// </summary>
        /// <param name="remoteAddress">URI containing the address of the WiniumDriver remote server (e.g. http://127.0.0.1:4444/wd/hub).</param>
        /// <param name="options">The <see cref="DriverOptions"/> object to be used with the Winium driver.</param>
        /// <param name="commandTimeout">The maximum amount of time to wait for each command.</param>
        public WiniumDriver(Uri remoteAddress, DriverOptions options, TimeSpan commandTimeout)
            : base(CommandExecutorFactory.GetHttpCommandExecutor(remoteAddress, commandTimeout), options.ToCapabilities())
        {
            this.InitWiniumDriverCommands();
            elementFactory = new WiniumElementFactory(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds the first element in the window that matches the <see cref="WiniumBy"/> object
        /// </summary>
        /// <param name="by">WiniumBy mechanism to find the object</param>
        /// <returns>Element</returns>
        public WiniumElement FindElement(WiniumBy by)
        {
            if (by == null)
            {
                throw new ArgumentNullException(nameof(@by), "by cannot be null");
            }

            return by.FindElement(this);
        }

        /// <summary>
        /// Finds an element matching the given mechanism and value.
        /// </summary>
        /// <param name="mechanism">The mechanism by which to find the element.</param>
        /// <param name="value">The value to use to search for the element.</param>
        /// <returns>Element</returns>
        public new WiniumElement FindElement(string mechanism, string value)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("using", mechanism);
            parameters.Add("value", value);
            Response commandResponse = this.Execute(DriverCommand.FindElement, parameters);
            return this.GetElementFromResponse(commandResponse);
        }

        /// <summary>
        /// Finds the elements on the page by using the <see cref="WiniumBy"/> object
        /// </summary>
        /// <param name="by">WiniumBy mechanism to find the object</param>
        /// <returns>Collection of elements</returns>
        public ReadOnlyCollection<WiniumElement> FindElements(WiniumBy by)
        {
            if (by == null)
            {
                throw new ArgumentNullException(nameof(@by), "by cannot be null");
            }

            return by.FindElements(this);
        }

        /// <summary>
        /// Finds all elements matching the given mechanism and value.
        /// </summary>
        /// <param name="mechanism">The mechanism by which to find the elements.</param>
        /// <param name="value">The value to use to search for the elements.</param>
        /// <returns>Collection of elements</returns>
        public new ReadOnlyCollection<WiniumElement> FindElements(string mechanism, string value)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("using", mechanism);
            parameters.Add("value", value);
            Response commandResponse = this.Execute(DriverCommand.FindElements, parameters);
            return this.GetElementsFromResponse(commandResponse);
        }

        public new IWebElement FindElement(By by) => throw new NotImplementedException();

        public new ReadOnlyCollection<IWebElement> FindElements(By by) => throw new NotImplementedException();

        /// <summary>
        /// Find the element in the response
        /// </summary>
        /// <param name="response">Response from the window</param>
        /// <returns>Element</returns>
        public WiniumElement GetElementFromResponse(Response response)
        {
            if (response == null)
            {
                throw new NoSuchElementException();
            }

            WiniumElement element = null;
            Dictionary<string, object> elementDictionary = response.Value as Dictionary<string, object>;
            if (elementDictionary != null)
            {
                element = this.elementFactory.CreateElement(elementDictionary);
            }

            return element;
        }

        /// <summary>
        /// Finds the elements that are in the response
        /// </summary>
        /// <param name="response">Response from the window</param>
        /// <returns>Collection of elements</returns>
        public ReadOnlyCollection<WiniumElement> GetElementsFromResponse(Response response)
        {
            List<WiniumElement> toReturn = new List<WiniumElement>();
            object[] elements = response.Value as object[];
            if (elements != null)
            {
                foreach (object elementObject in elements)
                {
                    Dictionary<string, object> elementDictionary = elementObject as Dictionary<string, object>;
                    if (elementDictionary != null)
                    {
                        WiniumElement element = this.elementFactory.CreateElement(elementDictionary);
                        toReturn.Add(element);
                    }
                }
            }

            return toReturn.AsReadOnly();
        }

        private static WiniumDriverService CreateDefaultService(Type optionsType, string directory)
        {
            if (optionsType == typeof(DesktopOptions))
            {
                return WiniumDriverService.CreateDesktopService(directory);
            }

            if (optionsType == typeof(StoreAppsOptions))
            {
                return WiniumDriverService.CreateStoreAppsService(directory);
            }

            if (optionsType == typeof(SilverlightOptions))
            {
                return WiniumDriverService.CreateSilverlightService(directory);
            }

            throw new ArgumentException(
                "Option type must be type of DesktopOptions, StoreAppsOptions or SilverlightOptions", 
                "optionsType");
        }

        private void InitWiniumDriverCommands()
        {
            this.CommandExecutor.TryAddCommand(
                "findDataGridCell", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/datagrid/cell/{row}/{column}"));

            this.CommandExecutor.TryAddCommand(
                "getDataGridColumnCount", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/datagrid/column/count"));

            this.CommandExecutor.TryAddCommand(
                "getDataGridRowCount", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/datagrid/row/count"));

            this.CommandExecutor.TryAddCommand(
                "scrollToDataGridCell", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/datagrid/scroll/{row}/{column}"));

            this.CommandExecutor.TryAddCommand(
                "selectDataGridCell", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/datagrid/select/{row}/{column}"));

            this.CommandExecutor.TryAddCommand(
                "scrollToListBoxItem", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/listbox/scroll"));

            this.CommandExecutor.TryAddCommand(
                "findMenuItem", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/menu/item/{path}"));

            this.CommandExecutor.TryAddCommand(
                "selectMenuItem", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/menu/select/{path}"));

            this.CommandExecutor.TryAddCommand(
                "isComboBoxExpanded", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/combobox/expanded"));

            this.CommandExecutor.TryAddCommand(
                "expandComboBox", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/combobox/expand"));

            this.CommandExecutor.TryAddCommand(
                "collapseComboBox", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/combobox/collapse"));

            this.CommandExecutor.TryAddCommand(
                "findComboBoxSelectedItem", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/combobox/items/selected"));

            this.CommandExecutor.TryAddCommand(
                "scrollToComboBoxItem", 
                new HttpCommandInfo("POST", "/session/{sessionId}/element/{id}/combobox/scroll"));

            this.CommandExecutor.TryAddCommand(
                "mouseDoubleClick",
                new HttpCommandInfo("POST", "/session/{sessionId}/doubleclick"));

            this.CommandExecutor.TryAddCommand(
                "mouseContextClick",
                new HttpCommandInfo("POST", "/session/{sessionId}/contextclick"));

            this.CommandExecutor.TryAddCommand(
                "mouseMoveTo",
                new HttpCommandInfo("POST", "/session/{sessionId}/moveto"));
        }

        #endregion
    }
}

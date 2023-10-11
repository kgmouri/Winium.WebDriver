namespace OpenQA.Selenium.Winium
{
    using System;
    #region using

    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    #endregion

    /// <summary>
    /// Element class for Winium.
    /// </summary>
    public class WiniumElement : WebElement, IFindsWiniumElement
    {
        #region Constants

        private const string MouseDoubleClick = "mouseDoubleClick";

        private const string MouseContextClick = "mouseContextClick";

        private const string MouseMoveTo = "mouseMoveTo";

        #endregion

        #region

        private WiniumDriver winiumDriver = null;

        #endregion

        #region Constructors

        public WiniumElement(WiniumDriver parentDriver, string id) : base(parentDriver, id)
        {
            winiumDriver = parentDriver;
        }

        #endregion

        #region Public Methods

        /// <see cref="WebElement.Displayed"/>
        public override bool Displayed
        {
            get
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("id", Id);
                Response response = Execute(DriverCommand.IsElementDisplayed, dictionary);
                return (bool)response.Value;
            }
        }

        /// <see cref="WebElement.GetAttribute"/>
        public override string GetAttribute(string attributeName)
        {
            Response commandResponse = null;
            string attributeValue = string.Empty;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("id", Id);
            parameters.Add("name", attributeName);
            commandResponse = Execute(DriverCommand.GetElementAttribute, parameters);

            if (commandResponse.Value == null)
            {
                attributeValue = null;
            }
            else
            {
                attributeValue = commandResponse.Value.ToString();

                // Normalize string values of boolean results as lowercase.
                if (commandResponse.Value is bool)
                {
                    attributeValue = attributeValue.ToLowerInvariant();
                }
            }

            return attributeValue;
        }

        /// <summary>
        /// Perform a double click.
        /// </summary>
        public void DoubleClick()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("id", Id);
            Execute(MouseDoubleClick, parameters);
        }

        /// <summary>
        /// Perform a context click.
        /// </summary>
        public void ContextClick()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("id", Id);
            Execute(MouseContextClick, parameters);
        }

        /// <summary>
        /// Perform a move to element.
        /// </summary>
        public void MoveToElement()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("element", Id);
            Execute(MouseMoveTo, parameters);
        }

        /// <summary>
        /// Moves the mouse to the specified offset of the top-left corner of the element.
        /// </summary>
        /// <param name="offsetX">The horizontal offset to which to move the mouse</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse</param>
        public void MoveToElement(int offsetX, int offsetY)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("element", Id);
            parameters.Add("xoffset", offsetX);
            parameters.Add("yoffset", offsetY);
            Execute(MouseMoveTo, parameters);
        }

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
            parameters.Add("id", this.Id);
            parameters.Add("using", mechanism);
            parameters.Add("value", value);
            Response commandResponse = this.Execute(DriverCommand.FindChildElement, parameters);
            return winiumDriver.GetElementFromResponse(commandResponse);
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
            parameters.Add("id", this.Id);
            parameters.Add("using", mechanism);
            parameters.Add("value", value);
            Response commandResponse = this.Execute(DriverCommand.FindChildElements, parameters);
            return winiumDriver.GetElementsFromResponse(commandResponse);
        }

        #endregion
    }
}

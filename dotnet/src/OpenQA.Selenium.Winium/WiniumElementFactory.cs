namespace OpenQA.Selenium.Winium
{
    #region using

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// ElementFactory class for Winium.
    /// </summary>
    public class WiniumElementFactory : WebElementFactory
    {
        #region Constructors

        public WiniumElementFactory(WiniumDriver parentDriver) : base(parentDriver)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an element from element information.
        /// </summary>
        /// <param name="elementDictionary">Element infomation</param>
        /// <returns>WiniumElement</returns>
        public new WiniumElement CreateElement(Dictionary<string, object> elementDictionary)
        {
            string elementId = GetElementId(elementDictionary);
            return new WiniumElement((WiniumDriver)ParentDriver, elementId);
        }

        #endregion
    }
}

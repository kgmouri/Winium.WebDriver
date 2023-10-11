namespace OpenQA.Selenium.Winium
{
    using OpenQA.Selenium.Internal;
    #region using

    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Linq;

    #endregion

    /// <summary>
    /// By class for Winium.
    /// </summary>
    public class WiniumBy : By
    {
        #region Constants

        private static readonly string IdMechanism = "id";

        private static readonly string ClassNameMechanism = "class name";

        private static readonly string NameMechanism = "name";

        private static readonly string XPathMechanism = "xpath";

        #endregion

        #region Fields

        private Func<ISearchContext, WiniumElement> findElementMethod;

        /// <summary>
        /// Gets or sets the method used to find a single element matching specified criteria.
        /// </summary>
        protected new Func<ISearchContext, WiniumElement> FindElementMethod
        {
            get { return this.findElementMethod; }
            set { this.findElementMethod = value; }
        }

        private Func<ISearchContext, ReadOnlyCollection<WiniumElement>> findElementsMethod;

        /// <summary>
        /// Gets or sets the method used to find all elements matching specified criteria.
        /// </summary>
        protected new Func<ISearchContext, ReadOnlyCollection<WiniumElement>> FindElementsMethod
        {
            get { return this.findElementsMethod; }
            set { this.findElementsMethod = value; }
        }

        #endregion

        #region Constructors

        private WiniumBy(string mechanism, string criteria) : base(mechanism, criteria) 
        {
            this.findElementMethod = (ISearchContext context) => ((IFindsWiniumElement)context).FindElement(mechanism, criteria);
            this.findElementsMethod = (ISearchContext context) => ((IFindsWiniumElement)context).FindElements(mechanism,criteria);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a By object for AutomationId.
        /// </summary>
        /// <param name="idToFind">AutomaitonId</param>
        /// <returns>By object for AutomationId</returns>
        public static WiniumBy AutomationId(string idToFind)
        {
            if (idToFind == null)
            {
                throw new ArgumentNullException("idToFind", "Cannot find elements with a null id attribute.");
            }

            var by = new WiniumBy(IdMechanism, idToFind);
            return by;
        }

        /// <summary>
        /// Create a By object for ClassName.
        /// </summary>
        /// <param name="classToFind">クラス名</param>
        /// <returns>By object for ClassName</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static new WiniumBy ClassName(string classToFind)
        {
            if (classToFind == null)
            {
                throw new ArgumentNullException("classToFind", "Cannot find elements with a null id attribute.");
            }

            var by = new WiniumBy(ClassNameMechanism, classToFind);
            return by;
        }

        /// <summary>
        /// Create a By object for Name.
        /// </summary>
        /// <param name="nameToFind">名前</param>
        /// <returns>By object for Name</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static new WiniumBy Name(string nameToFind)
        {
            if (nameToFind == null)
            {
                throw new ArgumentNullException("nameToFind", "Cannot find elements with a null id attribute.");
            }

            var by = new WiniumBy(NameMechanism, nameToFind);
            return by;
        }

        /// <summary>
        /// Create a By object for XPath.
        /// </summary>
        /// <param name="xpathToFind">XPath</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static new WiniumBy XPath(string xpathToFind)
        {
            if (xpathToFind == null)
            {
                throw new ArgumentNullException("xpathToFind", "Cannot find elements when the XPath expression is null.");
            }

            var by = new WiniumBy(XPathMechanism, xpathToFind);
            return by;
        }

        /// <see cref="By.Id"/>
        public static new WiniumBy Id(string idToFind) => throw new NotImplementedException();

        /// <see cref="By.LinkText"/>
        public static new WiniumBy LinkText(string linkTextToFind) => throw new NotImplementedException();

        /// <see cref="By.PartialLinkText"/>
        public static new WiniumBy PartialLinkText(string partialLinkTextToFind) => throw new NotImplementedException();

        /// <see cref="By.TagName"/>
        public static new WiniumBy TagName(string tagNameToFind) => throw new NotImplementedException();

        /// <see cref="By.CssSelector"/>
        public static new WiniumBy CssSelector(string cssSelectorToFind) => throw new NotImplementedException();

        /// <summary>
        /// Finds the first element matching the criteria.
        /// </summary>
        /// <param name="context">object to use to search for the elements.</param>
        /// <returns>Winium element</returns>
        public new WiniumElement FindElement(ISearchContext context)
        {
            return findElementMethod(context);
        }

        /// <summary>
        /// Finds all elements matching the criteria.
        /// </summary>
        /// <param name="context">object to use to search for the elements.</param>
        /// <returns>Winium elements</returns>
        public new ReadOnlyCollection<WiniumElement> FindElements(ISearchContext context)
        {
            return findElementsMethod(context);
        }

        #endregion
    }
}

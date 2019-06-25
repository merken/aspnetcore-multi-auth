namespace auth.api
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class RouteBranchAttribute : System.Attribute
    {
        // See the attribute guidelines at
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string route;

        // This is a positional argument
        public RouteBranchAttribute(string route)
        {
            this.route = route;
        }

        public string Route
        {
            get { return route; }
        }
    }
}
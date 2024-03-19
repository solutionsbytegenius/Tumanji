namespace Tumanji.Models
{
    public class CartItem
    {
        #region Vars
        public Guid ID { get; set; }
        public Guid PaninoID { get; set; }
        public bool Plus { get; set; } = false;
        public string Note { get; set; } = "";
        public string Bevanda { get; set; } = "";

        #endregion


        #region Costruttori
        /// <summary>
        /// Costruttore nullo
        /// </summary>
        public CartItem()
        {
            ID = new Guid();
            PaninoID = new Guid();
            Plus = false;
            Note = string.Empty;
            Bevanda = string.Empty;
        }
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="id"></param>
        /// <param name="panino"></param>
        /// <param name="plus"></param>
        /// <param name="note"></param>
        /// <param name="bevanda"></param>
        public CartItem( Guid panino, bool plus, string note, string bevanda)
        {
            ID = Guid.NewGuid();
            PaninoID = panino;
            Plus = plus;
            Note = note;
            Bevanda = bevanda;
        }
        #endregion
    }
}

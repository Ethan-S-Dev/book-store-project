using System;
using System.Collections;
using System.Collections.Generic;

namespace BookStore.ViewModel.Models
{
    public class GenresView : IEnumerable<GenreView>
    {
        private static GenresView allGenres;
        public static GenresView AllGenres { get { return new GenresView(new List<ViewGenres>());  } }
        private GenreView[] list;

        public int Count { get
            {
                int count = 0;
                foreach (var item in list)
                {
                    if (item.Active) count++;
                }
                return count;
            }
        }
        public int Length => list.Length;
        public GenreView this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        public IEnumerable<GenreView> Where(Predicate<GenreView> predicat)
        {
            LinkedList<GenreView> newList = new LinkedList<GenreView>();
            foreach (var item in list)
            {
                if (predicat(item))
                    newList.AddLast(item);
            }

            return newList;

        }
        public GenresView(List<ViewGenres> genresList)
        {
            list = new GenreView[Enum.GetValues(typeof(ViewGenres)).Length];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = new GenreView(i, Enum.GetName(typeof(ViewGenres), i), false);
            }

            foreach (var item in genresList)
            {
                list[(int)item].Active = true;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public IEnumerator<GenreView> GetEnumerator()
        {
            IEnumerable<GenreView> enuList = list;
            return enuList.GetEnumerator();
        }
    }
    public class GenreView
    {
        public int Id { get; set; }
        public string Name { get { return name; } set { name = value; } }
        public bool Active { get; set; }
        public string GenreToString { get { return ToString(); } }

        public GenreView(int id, string name, bool active)
        {
            Id = id;
            this.name = name;
            Active = active;
        }
        public override string ToString()
        {
            string[] spited = name.Split('_');
            if (spited.Length > 1)
                return $"{spited[0]} {spited[1]}";
            return name;

        }
        
        private string name;
    }

    public enum ViewGenres
    {
        None = 0, 
        Action,      
        Adventure,   
        Classic,     
        Comic_Book,  
        Graphic_Novel,
        Mystery,
        Fantasy,
        Horror,
        Romance,
        Historical_Fiction,
        Literary_Fiction,
        Science_Fiction,
        Short_Stories,
        Thriller,
        Biographies,
        Autobiographies
    }
}

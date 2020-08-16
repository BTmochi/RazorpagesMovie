using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public SelectList Genres { get; set; }
        //  警告
        // セキュリティ上の理由から、ページ モデルのプロパティに対して GET 要求データのバインドをオプトインする必要があります。 プロパティにマップする前に、ユーザー入力を確認してください。 GET バインドをオプトインするのは、クエリ文字列やルート値に依存するシナリオに対処する場合に便利です。
        // GET 要求のプロパティをバインドするには、[BindProperty] 属性の SupportsGet プロパティを true に設定します。
        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }

        public async Task OnGetAsync()
        {
            // using System.Linq;
            // データベースからすべてのジャンルを取得する LINQ クエリ
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                // ラムダ式
                // ラムダは、メソッド ベースの LINQ クエリで、Where メソッドや Contains(先のコードで使用されています) など、
                // 標準クエリ演算子メソッドの引数として使用されます。 LINQ クエリは、Where、Contains、OrderBy などのメソッドの呼び出しで定義または変更されたときには実行されません
                // クエリ実行は先送りされます。 つまり、その具体値が繰り返されるか、ToListAsync メソッドが呼び出されるまで、式の評価が延ばされます。

                // 注意
                // Contains メソッドは C# コードではなく、データベースで実行されます
                // クエリの大文字と小文字の区別は、データベースや照合順序に依存します。
                // SQL Server では、Contains は大文字/小文字の区別がない SQL LIKE にマッピングされます。
                // SQLite では、既定の照合順序で、大文字と小文字が区別されます。
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }
            // ジャンルの SelectList は、別個のジャンルを推定することで作成されます。
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}

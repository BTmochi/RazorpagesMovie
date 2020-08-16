using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace RazorPagesMovie.Models
{
    public class Movie
    {
        // IDフィールドは、データベースで主キーに必要
        public int ID { get; set; }
        public string Title { get; set; }

        // DataType 属性では、データの型 (Date) を指定します。
        // この属性を使用する場合:
        // ユーザーは日付フィールドに時刻の情報を入力する必要はありません。
        // 日付のみが表示され、時刻の情報は表示されません。
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string Rating { get; set; }
    }
}

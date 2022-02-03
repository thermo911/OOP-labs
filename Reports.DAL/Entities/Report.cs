using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Reports.DAL.Tools;

namespace Reports.DAL.Entities
{
    public class Report : IIdentifiable
    {
        private int _authorId;
        private string _text;
        private DateTime _fromDateTime;
        private DateTime _toDateTime;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AuthorId
        {
            get => _authorId;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(AuthorId)} is default");
                }
                
                _authorId = value;
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(Text)} is null or whitespace");
                }
                
                _text = value;
            }
        }

        public DateTime FromDateTime
        {
            get => _fromDateTime;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(FromDateTime)} is default");
                }

                _fromDateTime = value;
            }
        }

        public DateTime ToDateTime
        {
            get => _toDateTime;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException(
                        $"value assigned to property {nameof(ToDateTime)} is default");
                }

                _toDateTime = value;
            }
        }

        public DateTime CreationDateTime { get; set; }
    }
}

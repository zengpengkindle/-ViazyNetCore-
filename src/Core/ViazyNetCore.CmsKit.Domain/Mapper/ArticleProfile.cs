using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Modules.Mapper
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleEditDto, Article>().ReverseMap();
            CreateMap<Article, ArticleInfoDto>();

            CreateMap<CommentAddDto, Comment>().ReverseMap();
            CreateMap<Comment, CommentListItemDto>();
        }
    }
}

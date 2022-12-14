using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GCloud.App_Start;
using GCloud.Extensions;
using GCloud.Helper;
using GCloud.Models.Domain;
using GCloud.Repository;
using GCloud.Service;
using GCloud.Shared.Dto.Api;
using GCloud.Shared.Dto.Domain;
using GCloud.Shared.Exceptions.General;
using LinqKit;
using Microsoft.AspNet.Identity;

namespace GCloud.Controllers.api
{
    public class StoresApiController : ApiController
    {
        private readonly IStoreService _storeService;
        private readonly ITagRepository _tagRepository;

        public StoresApiController(IStoreService storeService, ITagRepository tagRepository)
        {
            _storeService = storeService;
            _tagRepository = tagRepository;
        }

        [Authorize(Roles = "Administrators,Managers")]
        [HttpGet]
        public HttpResponseMessage GetQrCode(Guid id)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var image = QrCodeHtmlHelper.GetQrCodeImage(id.ToString(), 500, 1);

            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                response.Content = new ByteArrayContent(ms.ToArray());
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return response;
            }
        }

        public List<StoreDto> Get()
        {
            return _storeService.FindAll().ToList().Select(x => Mapper.Map<StoreDto>(x, opts => opts.Items.Add(AutomapperConfig.UserId, User.Identity.GetUserId()))).ToList();
        }

        [Authorize]
        [Route("api/StoresApi/Tag")]
        [HttpGet]
        public List<StoreDto> FindByTag([FromUri]string[] tags)
        {
            if (tags == null || tags.Length == 0)
            {
                throw new GustavArgumentNullException(nameof(tags));
            }

            var predicate = PredicateBuilder.New<Tag>();

            foreach (var tag in tags)
            {
                predicate.And(x => x.Name.Contains(tag));
            }

            return _tagRepository.FindBy(predicate).SelectMany(x => x.Stores).ToList().Select(x => Mapper.Map<StoreDto>(x, opts =>
            {
                opts.Items.Add(AutomapperConfig.UserId, User.Identity.GetUserId());
            })).ToList();
        }

        [Authorize(Roles = "Managers")]
        public void Post(StoreManagerEditModel model)
        {
            var store = _storeService.FindById(model.Id);
            store.Name = model.Name;
            store.Street = model.Street;
            store.HouseNr = model.HouseNr;
            store.Plz = model.PostCode;
            store.City = model.City;
            _storeService.Update(store);
        }

        // TODO: This method has the same signature as the second method above?
        //[Authorize]
        //[Route("api/StoresApi/Tag")]
        //[HttpGet]
        //public List<StoreDto> FindByTag([FromUri]string[] tags)
        //{
        //    if (tags == null || tags.Length == 0)
        //    {
        //        throw new GustavArgumentNullException(nameof(tags));
        //    }

        //    var predicate = PredicateBuilder.New<Tag>();

        //    foreach (var tag in tags)
        //    {
        //        predicate.And(x => x.Name.Contains(tag));
        //    }

        //    return _tagRepository.FindBy(predicate).SelectMany(x => x.Stores).ProjectTo<StoreDto>().ToList();
        //}
    }
}
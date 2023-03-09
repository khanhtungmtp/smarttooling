using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Services.Services.SmartTool
{
    public class CrossSiteSharingService : ICrossSiteSharingService
    {
        private readonly IKaizenBenefitsApplicationFormRepository _formRepository;
        private readonly IModelRepository _repoModel;
        private readonly IKaizenRepository _kaizen;
        private readonly IModelOperationRepository _modelOperationRepository;
        private readonly IMapper _mapper;

        public CrossSiteSharingService(IKaizenBenefitsApplicationFormRepository formRepository,
                                        IModelRepository repoModel,
                                         IKaizenRepository kaizen,
                                        IModelOperationRepository modelOperationRepository,
                                        IMapper mapper)
        {
            _formRepository = formRepository;
            _repoModel = repoModel;
            _kaizen = kaizen;
            _modelOperationRepository = modelOperationRepository;
            _mapper = mapper;
        }
        public async Task<PagedList<CrossSiteSharingDTO>> Search(PaginationParams param, CrossSiteSharingParam filterParam)
        {
            var form = _formRepository.FindAll();
            var model = _repoModel.FindAll();
            var kaizen = _kaizen.FindAll();
            if (!string.IsNullOrEmpty(filterParam.factory_id))
            {
                form = form.Where(x => x.to_factory_id.Trim() == filterParam.factory_id.Trim());
            }
            if (!string.IsNullOrEmpty(filterParam.model_no))
            {
                model = model.Where(x => x.model_no.Contains(filterParam.model_no)
                || x.model_name.Contains(filterParam.model_no));
            }
            var data = (from a in form
                        join
         b in model on new { a.factory_id, a.model_no } equals new { b.factory_id, b.model_no }
                        join c in kaizen on new { a.factory_id, a.model_no, a.serial_no } equals
                        new { c.factory_id, c.model_no, c.serial_no }
                        select new CrossSiteSharingDTO()
                        {
                            doc_no = a.doc_no,
                            model_no = b.model_no,
                            model_name = b.model_name,
                            serial_no = a.serial_no,
                            kaizen_description = c.kaizen_description,
                            to_factory_id = a.to_factory_id,
                            IsChoise = false
                        }).OrderBy(x => x.doc_no == null ? 0 : 1).ThenByDescending(x => x.doc_no).ThenBy(x => x.model_no);
            return await PagedList<CrossSiteSharingDTO>.CreateAsync(data, param.PageNumber, param.PageSize);
        }
        public async Task<CrossSiteSharingEditDTO> GetCrossSiteSharingEdit(string factory, string modelNo, string serialNo)
        {
            var form = _formRepository.FindAll(x => x.to_factory_id.Trim() == factory.Trim()
            && x.model_no.Trim() == modelNo && x.serial_no == serialNo.ToInt());
            var model = _repoModel.FindAll();
            var kaizen = _kaizen.FindAll();
            var modelOperation = _modelOperationRepository.FindAll();
            var data = await (from a in form
                              join
         b in model on new { a.factory_id, a.model_no } equals new { b.factory_id, b.model_no }
                              join c in kaizen on new { a.factory_id, a.model_no, a.serial_no } equals
                              new { c.factory_id, c.model_no, c.serial_no }
                              join d in modelOperation on new { c.factory_id, c.model_no, c.operation_id } equals
                                   new { d.factory_id, d.model_no, d.operation_id }
                              select new CrossSiteSharingEditDTO()
                              {
                                  crossSiteSharingDTO = _mapper.Map<Kaizen_Benefits_Application_FormDTO>(a),
                                  kaizen_type_eliminate = c.kaizen_type_eliminate,
                                  kaizen_type_reduce = c.kaizen_type_reduce,
                                  kaizen_type_combine = c.kaizen_type_combine,
                                  kaizen_type_smart_tool = c.kaizen_type_smart_tool,
                                  kaizen_description = c.kaizen_description,
                                  before_media = c.before_media,
                                  after_media = c.after_media,
                                  ct_before_sec = c.ct_before_sec,
                                  ct_after_sec = c.ct_after_sec,
                                  rft_before_percent = c.rft_before_percent,
                                  rft_after_percent = c.rft_after_percent,
                                  model_name = b.model_name,
                                  operation_name_en = d.operation_name_en,
                                  operation_name_zh = d.operation_name_zh
                              }).ToListAsync();
            return _mapper.Map<List<CrossSiteSharingEditDTO>>(data).FirstOrDefault();
        }
        public async Task<List<CrossSiteSharingEditDTO>> GetCrossSiteSharingPDF(List<CrossSiteSharingDTO> filterParam)
        {
            filterParam = filterParam.Where(x => x.IsChoise == true).ToList();
            var form = await _formRepository.FindAll().ToListAsync();
            var model = _repoModel.FindAll();
            var kaizen = _kaizen.FindAll();
            var modelOperation = _modelOperationRepository.FindAll();
            List<CrossSiteSharingEditDTO> result = new List<CrossSiteSharingEditDTO>();
            filterParam.ForEach(item =>
            {
                var form1 = form.Where(x => x.to_factory_id.Trim() == item.to_factory_id.Trim()
                 && x.model_no.Trim() == item.model_no && x.serial_no == item.serial_no.ToInt());
                var data = (from a in form1
                            join
         b in model on new { a.factory_id, a.model_no } equals new { b.factory_id, b.model_no }
                            join c in kaizen on new { a.factory_id, a.model_no, a.serial_no } equals
                            new { c.factory_id, c.model_no, c.serial_no }
                            join d in modelOperation on new { c.factory_id, c.model_no, c.operation_id } equals
                                 new { d.factory_id, d.model_no, d.operation_id }
                            select new CrossSiteSharingEditDTO()
                            {
                                crossSiteSharingDTO = _mapper.Map<Kaizen_Benefits_Application_FormDTO>(a),
                                kaizen_type_eliminate = c.kaizen_type_eliminate,
                                kaizen_type_reduce = c.kaizen_type_reduce,
                                kaizen_type_combine = c.kaizen_type_combine,
                                kaizen_type_smart_tool = c.kaizen_type_smart_tool,
                                kaizen_description = c.kaizen_description,
                                before_media = c.before_media,
                                after_media = c.after_media,
                                ct_before_sec = c.ct_before_sec,
                                ct_after_sec = c.ct_after_sec,
                                rft_before_percent = c.rft_before_percent,
                                rft_after_percent = c.rft_after_percent,
                                model_name = b.model_name,
                                operation_name_en = d.operation_name_en,
                                operation_name_zh = d.operation_name_zh,
                                before_remarks = c.before_remarks,
                                after_remarks = c.after_remarks
                            }).ToList();
                result.Add(_mapper.Map<List<CrossSiteSharingEditDTO>>(data).FirstOrDefault());
            });
            return result;
        }

        public async Task<string> GetdocNo()
        {
            string doc_no = DateTime.Now.Year.ToString() + (DateTime.Now.Month < 10 ?
             ("0" + DateTime.Now.Month) : DateTime.Now.Month.ToString()) + (DateTime.Now.Day < 10 ?
              ("0" + DateTime.Now.Day) : DateTime.Now.Day.ToString());
            var item = await _formRepository.FindAll(x => x.doc_no.Contains(doc_no)).OrderByDescending(x => x.doc_no).FirstOrDefaultAsync();
            if (item != null)
            {
                var serinumber = item.doc_no.Substring(8).ToInt();
                var tmp = (serinumber >= 99) ? (serinumber + 1).ToString() : (serinumber < 9) ? ("00" + (serinumber + 1)) : ("0" + (serinumber + 1));
                doc_no = DateTime.Now.Year.ToString() + (DateTime.Now.Month < 10 ? ("0" + DateTime.Now.Month) : DateTime.Now.Month.ToString())
                + (DateTime.Now.Day < 10 ? ("0" + DateTime.Now.Day) : DateTime.Now.Day.ToString()) + tmp;
            }
            else
            {
                doc_no = DateTime.Now.Year.ToString() + (DateTime.Now.Month < 10 ? ("0" + DateTime.Now.Month) : DateTime.Now.Month.ToString())
                + (DateTime.Now.Day < 10 ? ("0" + DateTime.Now.Day) : DateTime.Now.Day.ToString()) + "001";
            }
            return doc_no;
        }

        public async Task<OperationResult> UpdateCrossSiteSharing(Kaizen_Benefits_Application_FormDTO model)
        {
            if (model.doc_no == "" || model.doc_no == null)
            {
                string doc_No = await GetdocNo();
                model.fill_in_date = DateTime.Now;
                model.doc_no = doc_No;
            }
            model.update_time = DateTime.Now;
            var modelUp = _mapper.Map<Kaizen_Benefits_Application_Form>(model);
            _formRepository.Update(modelUp);
            await _formRepository.SaveAll();
            return new OperationResult { Caption = "Success", Message = "Update Cross Site Sharing Success", Success = true };
        }
    }
}
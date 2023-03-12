export interface C2bLayoutAttachmentParam {
  line_name: string;
  line_type_name: string;
  model: string;
  prod_season: string;
}
export interface C2bLayoutAttachmentAddParam {
  model_name: string;
  line_id: string;
  line_type_id: string;
  model_no: string;
  prod_season: string;
  attachment_type_id: string;
  attachment_name: string;
  attachment_file_url: string;
}
export interface C2bLayoutAttachmentDeleteParam {
  attachment_file_url: string;
}

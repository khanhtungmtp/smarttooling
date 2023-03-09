import { BLLayoutDesignOverall } from './bl-layout-design-overall';
import { BLAttachmentType } from './bL_Attachment_Type';

export interface BLAttachments {
  id: number;
  layout_design_overall_id: number | null;
  attachment_type_id: string;
  attachment_name: string;
  attachment_file_url: string;
  update_by: string;
  update_time: string;
  attachment_type_: BLAttachmentType;
  layout_design_overall_: BLLayoutDesignOverall;
}

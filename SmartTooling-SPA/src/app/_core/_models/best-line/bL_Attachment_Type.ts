import { BLAttachments } from "./bl_attachments";

export interface BLAttachmentType {
  factory_id: string;
  attachment_type_id: string;
  attachment_type_name: string;
  sequence: number;
  is_active: boolean;
  update_by: string;
  update_time: string;
  bL_Attachments: BLAttachments[];
}

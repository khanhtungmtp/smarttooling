import { BL_Lines } from "./bl-lines";
import { BLAttachments } from "./bl_attachments";
import { BL_Critical_Process_Analysis } from "./bl_critical_process_analysis_audit";
import { BL_Layout_Design_Process } from "./bL_Layout_Design_Process";

export interface BLLayoutDesignOverall {
  id: number;
  factory_id: string;
  line_id: string;
  line_type_id: string;
  model_no: string;
  prod_season: string;
  no_of_process_before: number | null;
  no_of_process_after: number | null;
  tct_before: number;
  tct_after: number;
  cps_mp_before: number;
  cps_mp_after: number;
  assembly_mp_before: number;
  assembly_mp_after: number;
  eolr_before: number;
  eolr_after: number;
  ller_before_percent: number;
  ller_after_percent: number;
  tentative_pph_before: number;
  tentative_pph_after: number;
  tentative_efficiency_before_percent: number | null;
  tentative_efficiency_after_percent: number | null;
  c2b_overall_image: string;
  create_by: string;
  create_time: string;
  update_by: string;
  update_time: string;
  line_: BL_Lines;
  bL_Attachments: BLAttachments[];
  bL_Critical_Process_Analysis: BL_Critical_Process_Analysis[];
  bL_Layout_Design_Process: BL_Layout_Design_Process[];
}

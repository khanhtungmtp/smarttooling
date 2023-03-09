import { BLLayoutDesignOverall } from "./bl-layout-design-overall";

export interface BL_Lines {
  factory_id: string;
  line_id: string;
  line_name: string;
  sequence: number;
  is_active: boolean;
  update_by: string;
  update_time: string;
  bL_Layout_Design_Overall: BLLayoutDesignOverall[];
}

export interface ModelCrossSiteSharing {
    factory_id: string;
	model_no: string;
	serial_no: number;
	to_factory_id: string;
	doc_no: string;
	fill_in_date: string | null;
	proposed_by_facility: string;
	proposed_by_dept: string;
	proposed_by_name_id: string;
	team_member: string;
	benefits_category_hse: boolean | null;
	benefits_category_delivery: boolean | null;
	benefits_category_quality: boolean | null;
	benefits_category_efficiency: boolean | null;
	benefits_category_others: boolean | null;
	estimated_savings_per_month: number | null;
	estimated_roi: number | null;
	create_by: string;
	create_time: string;
	update_by: string;
	update_time: string;
    isChoise:boolean;
	kaizen_description: string;
	model_name:string;
}
export class ModelCrossSiteSharingEdit{
	crossSiteSharingDTO:ModelCrossSiteSharing;
	kaizen_type_eliminate: boolean;
    kaizen_type_reduce: boolean;
    kaizen_type_combine: boolean;
    kaizen_type_smart_tool: boolean;
	kaizen_description: string;
	before_media: string;
    after_media: string;
	ct_before_sec: number;
    ct_after_sec: number;
    rft_before_percent: number;
    rft_after_percent: number;
	model_name:string;
	operation_name_local:string;
	operation_name_en:string;
	before_remarks:string;
	after_remarks:string;
}
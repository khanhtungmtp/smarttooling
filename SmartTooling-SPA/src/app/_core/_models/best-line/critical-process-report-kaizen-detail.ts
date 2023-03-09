import { Kaizen } from "../smart-tool/kaizen";
import { Model } from "../smart-tool/model";
import { ModelOperation } from "../smart-tool/model-operation";

export interface CriticalProcessReportKaizenDetail {
    kaizen: Kaizen;
    model: Model;
    modelOperation: ModelOperation;
}
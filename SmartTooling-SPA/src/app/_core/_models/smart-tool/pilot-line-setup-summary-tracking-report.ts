export interface PilotLineSetupSummaryTrackingReport {
    factory: string;
    prod_season: string;
    target_number_of_pilot_lines_setup: number;
    actual_number_of_pilot_lines_setup: number;
    pilot_lines_setup_percent: number;
    pilot_line: string;
    target_number_of_rollout_lines_setup_for_pilot_lines: number;
    actual_number_of_rollout_lines_setup_for_pilot_lines: number;
    rollout_lines_setup_percent_for_pilot_lines: number;
}

export interface PilotLineSetupSummaryTrackingGroupReport {
    factory: string;
    data: PilotLineSetupSummaryTrackingReport
}
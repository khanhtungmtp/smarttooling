import { ServerInfo } from "../_models/server-info";
import { SharedResourcesService } from "../_services/shared-resources.service";

export function appInitializer(sharedResourcesService: SharedResourcesService) {
  return () => new Promise(resolve => {
    sharedResourcesService.getServerInfo()
      .subscribe({
        next: (res: ServerInfo) => {
          localStorage.setItem('factorySmartTooling', res.factory);
        }
      })
      .add(resolve);
  });
}
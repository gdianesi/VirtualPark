export interface CreateRewardRequest {
  name: string;
  description: string;
  cost: number;
  quantityAvailable: number;
  requiredMembershipLevel: string;
}

export interface RewardModel {
  id: string;
  name: string;
  description: string;
  cost: number;
  quantityAvailable: number;
  requiredMembershipLevel: string;
}
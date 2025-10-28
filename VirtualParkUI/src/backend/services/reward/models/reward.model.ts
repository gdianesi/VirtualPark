export interface Reward {
  id?: string;
  name: string;
  description: string;
  cost: number;
  quantityAvailable: number;
  requiredMembershipLevel: string;
}
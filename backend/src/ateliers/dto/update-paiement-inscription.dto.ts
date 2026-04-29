import { ApiProperty } from '@nestjs/swagger';
import { IsEnum } from 'class-validator';
import { StatutPaiement } from 'generated/types/enums';

export class UpdatePaiementInscriptionDto {
  @ApiProperty({ enum: StatutPaiement, description: 'Statut de paiement' })
  @IsEnum(StatutPaiement)
  readonly statutPaiement!: StatutPaiement;
}

-- CreateEnum
CREATE TYPE "StatutPaiement" AS ENUM ('REGLE', 'A_REGLER');

-- AlterTable
ALTER TABLE "ateliers" ADD COLUMN     "prix" DECIMAL(10,2);

-- AlterTable
ALTER TABLE "inscriptions_ateliers" ADD COLUMN     "statutPaiement" "StatutPaiement" NOT NULL DEFAULT 'A_REGLER';

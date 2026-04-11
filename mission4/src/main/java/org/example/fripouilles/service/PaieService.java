package org.example.fripouilles.service;

import com.google.gson.JsonObject;
import com.google.gson.reflect.TypeToken;
import org.example.fripouilles.model.Paie;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.List;

/**
 * Service pour gérer les opérations liées aux fiches de paie (Admin uniquement)
 */
public class PaieService {
    private final ApiService apiService;

    public PaieService() {
        this.apiService = new ApiService();
    }

    /**
     * Récupère toutes les fiches de paie (admin)
     */
    public List<Paie> getAllPaies(Integer annee, Integer mois) throws IOException {
        String endpoint = "/paie";

        if (annee != null || mois != null) {
            endpoint += "?";
            if (annee != null) {
                endpoint += "annee=" + annee;
            }
            if (mois != null) {
                if (annee != null) endpoint += "&";
                endpoint += "mois=" + mois;
            }
        }
        
        Type listType = new TypeToken<List<Paie>>(){}.getType();
        return apiService.get(endpoint, listType);
    }

    /**
     * Récupère une fiche de paie par ID (admin)
     */
    public Paie getPaieById(int id) throws IOException {
        return apiService.get("/paie/" + id, Paie.class);
    }

    /**
     * Génère une fiche de paie
     */
    public Paie genererPaie(int contratId, int mois, int annee, String commentaire) throws IOException {
        JsonObject request = new JsonObject();
        request.addProperty("contratId", contratId);
        request.addProperty("mois", mois);
        request.addProperty("annee", annee);
        if (commentaire != null && !commentaire.isEmpty()) {
            request.addProperty("commentaire", commentaire);
        }
        
        return apiService.post("/paie/generer", request, Paie.class);
    }

    /**
     * Supprime une fiche de paie (admin)
     */
    public void deletePaie(int id) throws IOException {
        apiService.delete("/paie/" + id);
    }
}

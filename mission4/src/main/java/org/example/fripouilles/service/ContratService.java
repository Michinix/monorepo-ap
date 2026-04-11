package org.example.fripouilles.service;

import com.google.gson.reflect.TypeToken;
import org.example.fripouilles.model.Contrat;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.List;

/**
 * Service pour gérer les opérations liées aux contrats de garde (Admin uniquement)
 */
public class ContratService {
    private final ApiService apiService;

    public ContratService() {
        this.apiService = new ApiService();
    }

    /**
     * Récupère tous les contrats (admin)
     */
    public List<Contrat> getAllContrats() throws IOException {
        Type listType = new TypeToken<List<Contrat>>(){}.getType();
        return apiService.get("/contrat-garde/admin/tous", listType);
    }

    /**
     * Récupère un contrat par ID (admin)
     */
    public Contrat getContratById(int id) throws IOException {
        return apiService.get("/contrat-garde/" + id, Contrat.class);
    }
}

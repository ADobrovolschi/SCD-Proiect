package com.utcn.ProiectSCD.post;


import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.utcn.ProiectSCD.user.User;
import com.utcn.ProiectSCD.comment.Comment;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.CreationTimestamp;

import java.util.Date;
import java.util.List;

@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Post {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    private String title;

    private String content;

    @CreationTimestamp
    @Column(updatable = false, nullable = false)
    private Date createdOn;

    private Status status;

    @ManyToOne
    @JoinColumn(name="user_id", nullable = true)
    private User user;

    @OneToMany(mappedBy = "post", cascade = CascadeType.ALL)
    @JsonIgnoreProperties("post") // Previne recursivitatea infinită
    private List<Comment> comments;
}
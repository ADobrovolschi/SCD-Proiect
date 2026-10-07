package com.utcn.ProiectSCD.email;

import com.utcn.ProiectSCD.post.Post;
import jakarta.mail.MessagingException;
import jakarta.mail.internet.MimeMessage;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;
import org.springframework.stereotype.Service;

@Service
@Slf4j
public class EmailService {

    @Autowired
    private JavaMailSender emailSender;

    public void sendPostNotification(Post post) {
        try {
            log.info("Attempting to send email to: " + post.getUser().getEmail());

            MimeMessage message = emailSender.createMimeMessage();
            MimeMessageHelper helper = new MimeMessageHelper(message, true);

            helper.setFrom("alexandudobrovolschi@gmail.com");
            helper.setTo(post.getUser().getEmail());
            helper.setSubject("New Post Created: " + post.getTitle());

            String htmlContent = String.format("""
                <h2>Your post has been created successfully!</h2>
                <p>Title: %s</p>
                <p>Content: %s</p>
                <p>Status: %s</p>
                <p>Created on: %s</p>
                """,
                    post.getTitle(),
                    post.getContent(),
                    post.getStatus(),
                    post.getCreatedOn()
            );

            helper.setText(htmlContent, true);
            emailSender.send(message);
            log.info("Email sent successfully!");

        } catch (MessagingException e) {
            log.error("Failed to send email: " + e.getMessage(), e);
            throw new RuntimeException("Failed to send email", e);
        }
    }
}
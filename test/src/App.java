import java.time.LocalDateTime;
import java.io.*;
import javax.swing.*;

public class App {
    static JFrame frame = new JFrame("Tim stinkt");

    static File file = new File("hahaha.txt");
    static BufferedWriter writer = new BufferedWriter(new FileWriter(file));
    public static void main(String[] args) {
        System.out.println("Hello, World!");
        System.out.println("Hello, World!");
        System.out.println("Hello, World!");
        System.out.println(LocalDateTime.now());
        frame.setSize(500, 500);
        frame.setVisible(true);
        BufferedWriter writer = new BufferedWriter(new FileWriter("hahaha.txt"));        
    }

    public static LocalDateTime date(){
        LocalDateTime time = new LocalDateTime();
        return time;
    }
}
